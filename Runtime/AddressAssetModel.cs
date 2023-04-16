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
    public class AddressAssetModel
    {
        [SerializeField] private string name;
        private readonly string key;
        private readonly AsyncOperationHandle<object> reference;
        private object value;
        public int Count { get; private set; }

        public AddressAssetModel(string key)
        {
            this.name = key;
            this.key = key;
            reference = Addressables.LoadAssetAsync<object>(key);
            this.value = null;
            this.Count = 0;
        }
        public async Task<object> Load()
        {
            if (value == null)
            {
                value = await reference.Task;
                if (value == null)
                {
                    return value;
                }
                else
                {
                    this.name = $"{key} : {value.GetType().Name}";
                }
            }
            Count++;
            return value;
        }
        public void Release()
        {
            if (Count == 1)
            {
                Addressables.Release(reference);
            }
            if (Count > 0)
            {
                Count--;
            }
        }
        public void Remove()
        {
            Count = 0;
            Addressables.Release(reference);
        }
    }
}