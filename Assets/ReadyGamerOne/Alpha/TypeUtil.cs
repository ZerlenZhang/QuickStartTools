using System.Collections.Generic;
using System.Reflection;
using ReadyGamerOne.View;
using UnityEngine;

namespace UnityEditor
{
    public static class TypeUtil
    {
        public static void GetAllTypeInfo()
        {
            var text = "";
            var allType = Assembly.GetCallingAssembly().GetTypes();
            foreach (var VARIABLE in allType)
            {
                if(VARIABLE.IsSubclassOf(typeof(AbstractPanel)))
                    text += VARIABLE.Name+"\n";
            }

            Debug.Log(text);
        }

        private static void ShowTypeFunc()
        {
            var aniType = typeof(Ani);
            var catType = typeof(Cat);
            var list_int_Type = typeof(List<int>);
            var listArguments= list_int_Type.GenericTypeArguments;
            var text = "";
            foreach (var VARIABLE in listArguments)
            {
                text += "List<int> GenericTypeArguments:  " + VARIABLE.FullName;
            }

            Debug.Log(text);
            
            
            var list_Ani_Type = typeof(List<Ani>);
            var mylist_Ani_Type = typeof(MyList<Ani>);
            var mylist_Cat_Type = typeof(MyList<Cat>);
            
            Debug.Log("MyList<Ani> is child of List<Ani>:"+(list_Ani_Type.IsAssignableFrom(mylist_Ani_Type)));
            Debug.Log("MyList<Cat> is child of List<Ani>:"+(list_Ani_Type.IsAssignableFrom(mylist_Cat_Type)));
            Debug.Log("MyList<Cat> is child of MyList<Ani>:"+(mylist_Ani_Type.IsAssignableFrom(mylist_Cat_Type)));
            

        }
        
        
    }

    #region ExampleClasses

    public class MyList<T> : List<T>
    {
        
    }


    internal class Ani
    {
        public virtual string Name => "AniStr";
    }

    internal class Cat : Ani
    {
        public override string Name => "CatStr";
    }

    internal class Dog : Ani
    {
        public override string Name => "DogStr";
    }
    
    

    #endregion
}