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
using System;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace UniFlux.Addresables
{
    [Serializable]
    public class AddressGameObjectModel
    {
        [SerializeField] private string name;
        public readonly string key;
        public readonly AsyncOperationHandle<GameObject> reference;
        public GameObject instance;
        public AddressGameObjectModel(in (string key, Transform parent, Vector3 position, Quaternion rotation) data)
        {
            this.name = data.key;
            this.key = data.key;
            this.reference = Addressables.InstantiateAsync(data.key, data.position, data.rotation, data.parent);
        }
        public async Task<GameObject> Load()
        {
            if (instance == null)
            {
                instance = await reference.Task;
                if (instance == null)
                {
                    return instance;
                }
                else
                {
                    this.name = $"{key}";
                    instance.name = key;
                }
            }
            return instance;
        }
        public void Release()
        {
            Addressables.ReleaseInstance(reference);
        }
        public void Remove()
        {
            Addressables.Release(reference);
        }
    }
}