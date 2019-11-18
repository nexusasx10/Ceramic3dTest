using UnityEngine;

namespace Ceramic3d.Test
{
    public class Itemlist : MonoBehaviour
    {
        public Item[] Items;
        public GameObject ItemPrefab;

        private void Start()
        {
            foreach (var item in Items)
            {
                var itemObject = Instantiate(ItemPrefab, transform);
                itemObject.GetComponent<ItemView>().Prefab = item.gameObject;
            }
        }
    }
}
