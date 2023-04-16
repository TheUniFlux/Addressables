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
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine;
using Kingdox.UniFlux;
namespace UniFlux.Addresables
{
    public sealed class Addressable_Module : MonoFlux
    {
        private Dictionary<string, AddressAssetModel> dic_assets = new Dictionary<string, AddressAssetModel>();
        [SerializeField] private List<AddressSceneModel> list_sceneModel = new List<AddressSceneModel>();
        private Dictionary<string, List<AddressGameObjectModel>> dic_list_gameobjectModel = new Dictionary<string, List<AddressGameObjectModel>>();
        private void OnDestroy()
        {
            __ForceRelease();
        }
#if UNITY_EDITOR
        [SerializeField] private List<AddressAssetModel> __list_dic = new List<AddressAssetModel>();
        [SerializeField] private List<AddressGameObjectModel> __list_instances = new List<AddressGameObjectModel>();
        private void OnGUI()
        {
            if (!Application.isPlaying) return;
            __list_dic.Clear();
            foreach (var item in dic_assets) __list_dic.Add(item.Value);
            __list_instances.Clear();
            foreach (var item in dic_list_gameobjectModel) __list_instances.AddRange(item.Value);
        }
#endif
        private void __ForceRelease()
        {
            // Release all
            foreach (var item in dic_assets) item.Value.Remove();
            // Release list of scenes in addressables
            foreach (var item in list_sceneModel) item.Remove();
            // Release all the GameObject
            foreach (var item in dic_list_gameobjectModel)
            {
                foreach (var _item in item.Value) _item.Remove();
            }
        }
        [Flux(AddressableService.Key.Initialize)] private Task Initialize() 
        {
            return Addressables.InitializeAsync().Task;
        }
        [Flux(AddressableService.Key.LoadScene)] private IEnumerator LoadScene(string scene)
        {
            list_sceneModel.Add(new AddressSceneModel(scene));
            yield return list_sceneModel.Find(s => s.key.Equals(scene)).Load();
        }
        [Flux(AddressableService.Key.UnLoadScene)] private IEnumerator UnLoadScene(string scene)
        {
            var _ref = list_sceneModel.Find(s => s.key.Equals(scene));
            list_sceneModel.Remove(_ref);
            yield return _ref.Release();
        }
        [Flux(AddressableService.Key.LoadGameObject)] private async Task<GameObject> LoadGameObject((string key, Transform parent, Vector3 position, Quaternion rotation) data)
        {
            var model = new AddressGameObjectModel(data);
            if (dic_list_gameobjectModel.TryGetValue(data.key, out List<AddressGameObjectModel> _list))
            {
                _list.Add(model);
            }
            else
            {
                var __list = new List<AddressGameObjectModel>();
                __list.Add(model);
                dic_list_gameobjectModel.Add(data.key, __list);
            }
            return await model.Load();
        }
        [Flux(AddressableService.Key.UnloadGameObject)] private void UnloadGameObject((string key, GameObject obj)  data)
        {
            if (dic_list_gameobjectModel.TryGetValue(data.key, out List<AddressGameObjectModel> _list))
            {
                var _obj = _list?.Find(g => g.instance.Equals(data.obj));
                if (_obj is null)
                {
                    // Debug.LogWarning("Error Unloading, No exist in List<>");
                }
                else
                {
                    _obj.Release();
                    _list.Remove(_obj);
                    if (_list.Count.Equals(0)) dic_list_gameobjectModel.Remove(data.key);
                }
            }
            else
            {
                //Si no encuentra no pasa nada
            }
        }
        [Flux(AddressableService.Key.UnloadGameObject)] private void UnloadGameObject(string key)
        {
            if (dic_list_gameobjectModel.TryGetValue(key, out List<AddressGameObjectModel> _list))
            {
                for (int i = 0; i < _list.Count; i++) _list[i].Release();
                dic_list_gameobjectModel.Remove(key);
            }
        }
        [Flux(AddressableService.Key.LoadAsset)] private async Task<object> LoadAsset(string key)
        {
            if (dic_assets.TryGetValue(key, out AddressAssetModel address_Model))
            {
                return await address_Model.Load();
            }
            else
            {
                dic_assets.Add(key, new AddressAssetModel(key));
                object _value = await dic_assets[key].Load();
                if (_value == null)
                {
                    dic_assets.Remove(key);
                }
                return _value;
            }
        }
        [Flux(AddressableService.Key.UnLoadAsset)] private void UnLoadAsset(string key)
        {
            if (dic_assets.TryGetValue(key, out AddressAssetModel value))
            {
                value.Release();
                if (value.Count <= 0)
                {
                    dic_assets.Remove(key);
                }
            }
            else
            {
                //No hay nada que soltar, no pasa na
            }
        }
        [Flux(AddressableService.Key.Exist)] private bool Exist(string key) 
        {
            return dic_assets.ContainsKey(key) || list_sceneModel.Find(s => s.key.Equals(key)) != null;
        }
    }
}