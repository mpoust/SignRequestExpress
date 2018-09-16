////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: TypeInfoMemberExtensions.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/16/2018
 * Last Modified:
 * Description: Helper code for inspecting a response to find any Links that can be rewritten within LinkRewritingFilter.
 * 
 * References:  
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Reflection;

namespace SignRequestExpressAPI
{
    public static class TypeInfoMemberExtensions
    {
        public static IEnumerable<ConstructorInfo> GetAllConstructors(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredConstructors);

        public static IEnumerable<EventInfo> GetAllEvents(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredEvents);

        public static IEnumerable<FieldInfo> GetAllFields(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredFields);

        public static IEnumerable<MemberInfo> GetAllMembers(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredMembers);

        public static IEnumerable<MethodInfo> GetAllMethods(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredMethods);

        public static IEnumerable<TypeInfo> GetAllNestedTypes(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredNestedTypes);

        public static IEnumerable<PropertyInfo> GetAllProperties(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredProperties);

        private static IEnumerable<T> GetAll<T>(this TypeInfo typeInfo, Func<TypeInfo, IEnumerable<T>> accessor)
        {
            while (typeInfo != null)
            {
                foreach (var t in accessor(typeInfo))
                {
                    yield return t;
                }

                typeInfo = typeInfo.BaseType?.GetTypeInfo();
            }
        }
    }
}
