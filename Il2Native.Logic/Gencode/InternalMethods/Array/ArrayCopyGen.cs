﻿namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ArrayCopyGen
    {
        public static readonly string Name = "Void System.Array.Copy(System.Array, Int32, System.Array, Int32, Int32)";

        public static readonly object[] ByteCode =
        {
            Code.Ldarg_0,
            Code.Castclass,
            5,
            0,
            0,
            0,
            Code.Ldfld,
            2,
            0,
            0,
            0,
            Code.Stloc_0,
            Code.Ldarg_2,
            Code.Castclass,
            5,
            0,
            0,
            0,
            Code.Ldarg_3,
            Code.Ldloc_0,
            Code.Mul,
            Code.Ldelema,
            4,
            0,
            0,
            0,
            Code.Ldarg_0,
            Code.Castclass,
            5,
            0,
            0,
            0,
            Code.Ldarg_1,
            Code.Ldloc_0,
            Code.Mul,
            Code.Ldelema,
            4,
            0,
            0,
            0,
            Code.Ldarg_S,
            4,
            Code.Ldloc_0,
            Code.Mul,
            Code.Call,
            3,
            0,
            0,
            0,
            Code.Ret
        };

        public static void Register(ITypeResolver typeResolver)
        {
            var arrayType = typeResolver.System.System_Byte.ToArrayType(1);

            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(typeResolver.System.System_Byte.ToPointerType());
            tokenResolutions.Add(arrayType.GetFieldByName("elementSize", typeResolver));
            tokenResolutions.Add(
                new SynthesizedInlinedTextMethod(
                    string.Empty,
                    typeResolver.System.System_Array,
                    typeResolver.System.System_Void,
                    new[]
                    {
                        typeResolver.System.System_Byte.ToPointerType().ToParameter("src"),
                        typeResolver.System.System_Byte.ToPointerType().ToParameter("dst"),
                        typeResolver.System.System_Int32.ToParameter("len")
                    },
                    (llvmWriter, opCode) =>
                    {
                        // copy data
                        var firstByteOfSourceArray = opCode.OpCodeOperands[0];
                        var firstByteOfDestArray = opCode.OpCodeOperands[1];
                        var len = opCode.OpCodeOperands[2];
                        llvmWriter.WriteMemCopy(firstByteOfSourceArray, firstByteOfDestArray, len);
                    }));
            tokenResolutions.Add(typeResolver.System.System_Byte);
            tokenResolutions.Add(arrayType);

            var locals = new List<IType>();
            locals.Add(typeResolver.System.System_Int32);

            var parameters = new List<IParameter>();
            parameters.Add(typeResolver.System.System_Array.ToParameter("src"));
            parameters.Add(typeResolver.System.System_Int32.ToParameter("srcIndex"));
            parameters.Add(typeResolver.System.System_Array.ToParameter("dst"));
            parameters.Add(typeResolver.System.System_Int32.ToParameter("dstIndex"));
            parameters.Add(typeResolver.System.System_Int32.ToParameter("len"));

            MethodBodyBank.Register(Name, ByteCode, tokenResolutions, locals, parameters);
        }
    }
}