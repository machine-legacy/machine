using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Machine.MassTransitExtensions.InterfacesAsMessages
{
  public class MessageInterfaceImplementations
  {
    private readonly Dictionary<Type, Type> _interfaceToClass = new Dictionary<Type, Type>();
    private readonly Dictionary<Type, Type> _classToInterface = new Dictionary<Type, Type>();
    private AssemblyBuilder _assemblyBuilder;
    private ModuleBuilder _moduleBuilder;

    public void GenerateImplementationsOf(params Type[] types)
    {
      GenerateImplementationsOf(new List<Type>(types));
    }

    public void GenerateImplementationsOf(IEnumerable<Type> types)
    {
      string name = "Messages";
      AssemblyName assemblyName = new AssemblyName();
      assemblyName.Name = name;
      _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
      _moduleBuilder = _assemblyBuilder.DefineDynamicModule(name, name + ".dll");
      foreach (Type type in types)
      {
        if (!type.IsInterface)
        {
          throw new InvalidOperationException();
        }
        GenerateStub(type);
      }
    }

    private void GenerateStub(Type type)
    {
      string newTypeName = MakeImplementationName(type);
      TypeAttributes attributes = TypeAttributes.Public | TypeAttributes.Serializable;
      TypeBuilder typeBuilder = _moduleBuilder.DefineType(newTypeName, attributes);
      typeBuilder.AddInterfaceImplementation(type);
      foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
      {
        if (!property.CanRead || !property.CanWrite)
        {
          throw new InvalidOperationException();
        }
        FieldBuilder fieldBuilder = typeBuilder.DefineField(MakeFieldName(property), property.PropertyType, FieldAttributes.Private);
        PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(property.Name, PropertyAttributes.None, property.PropertyType, new Type[0]);
        MethodBuilder getMethod = typeBuilder.DefineMethod("get_" + propertyBuilder.Name, MethodAttributes.Virtual | MethodAttributes.Public, property.PropertyType, new Type[0]);
        ILGenerator ilGet = getMethod.GetILGenerator();
        ilGet.Emit(OpCodes.Ldarg_0);
        ilGet.Emit(OpCodes.Ldfld, fieldBuilder);
        ilGet.Emit(OpCodes.Ret);

        propertyBuilder.SetGetMethod(getMethod);
        typeBuilder.DefineMethodOverride(getMethod, property.GetGetMethod());

        MethodBuilder setMethod = typeBuilder.DefineMethod("set_" + propertyBuilder.Name, MethodAttributes.Virtual | MethodAttributes.Public, typeof(void), new Type[] { property.PropertyType });
        ParameterBuilder parameterBuilder = setMethod.DefineParameter(0, ParameterAttributes.None, "value");
        ILGenerator ilSet = setMethod.GetILGenerator();
        ilSet.Emit(OpCodes.Ldarg_0);
        ilSet.Emit(OpCodes.Ldarg_1);
        ilSet.Emit(OpCodes.Stfld, fieldBuilder);
        ilSet.Emit(OpCodes.Ret);

        propertyBuilder.SetSetMethod(setMethod);
        typeBuilder.DefineMethodOverride(setMethod, property.GetSetMethod());
      }
      _interfaceToClass[type] = typeBuilder.CreateType();
      _classToInterface[_interfaceToClass[type]] = type;
    }

    private static string MakeFieldName(PropertyInfo property)
    {
      return "_" + property.Name;
    }

    private static string MakeImplementationName(Type type)
    {
      string name = type.Name;
      if (name.StartsWith("I"))
      {
        name = name.Substring(1);
      }
      return "A" + name;
    }

    public Type GetClassFor(Type type)
    {
      return _interfaceToClass[type];
    }

    public Type GetInterfaceFor(Type type)
    {
      return _classToInterface[type];
    }

    public bool IsClassOrInterface(Type type)
    {
      return _interfaceToClass.ContainsKey(type) || _classToInterface.ContainsKey(type);
    }
  }
}