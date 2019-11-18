using UnityEngine;
using UnityEngine.UI;

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
                var itemView = itemObject.GetComponent<ItemView>();
                itemView.Prefab = item.gameObject;
                itemView.transform.Find("Image").GetComponentInChildren<Image>().sprite = item.Image;
                itemView.GetComponentInChildren<Text>().text = item.Name;
            }
        }
    }
}
