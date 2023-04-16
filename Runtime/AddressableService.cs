/*
Copyright (c) 2023 Xavier Arpa López Thomas Peter ('Kingdox')

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Kingdox.UniFlux;
namespace UniFlux.Addresables
{
    public static partial class AddressableService // Data
    {
        private static partial class Data
        {
            
        }
    }
    public static partial class AddressableService // Key
    {
        public static partial class Key
        {
            private const string _AddressableService =  nameof(AddressableService) + ".";
            public const string Initialize = _AddressableService + nameof(Initialize);
            public const string LoadAsset = _AddressableService + nameof(LoadAsset);
            public const string UnLoadAsset = _AddressableService + nameof(UnLoadAsset);
            public const string LoadScene = _AddressableService + nameof(LoadScene);
            public const string UnLoadScene = _AddressableService + nameof(UnLoadScene);
            public const string Exist = _AddressableService + nameof(Exist);
            public const string LoadGameObject = _AddressableService + nameof(LoadGameObject);
            public const string UnloadGameObject = _AddressableService + nameof(UnloadGameObject);
        }
    }
    public static partial class AddressableService // Methods
    {
        public static Task Initialize() => Key.Initialize.Task();
        public static Task<object> Load(string data) => Key.LoadAsset.Task<string,object>(in data);
        public static void UnLoad(in string data) => Key.UnLoadAsset.Dispatch(in data);
        public static IEnumerator LoadScene(in string data) => Key.LoadScene.IEnumerator(in data);
        public static IEnumerator UnloadScene(in string data) => Key.UnLoadScene.IEnumerator(in data);
        public static Task<GameObject> LoadGameObject(in (string key, Transform parent, Vector3 position, Quaternion rotation) data) => Key.LoadGameObject.Task<(string key, Transform parent, Vector3 position, Quaternion rotation),GameObject>(in data);
        public static void UnloadGameObject(in (string key, GameObject obj) data) => Key.UnloadGameObject.Dispatch(data);
        public static void UnloadGameObject(in string data) => Key.UnloadGameObject.Dispatch(data);
        public static bool Exist(in string key) => Key.Exist.Dispatch<string, bool>(key);
    }
}