using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Factories
{
    class ScrollViewItemFactory
    {
        public GameObject ItemPrefab = Configuration.Configuration.Instance.ScrollViewItemPrefab;

        public int OffsetBetweenItems = 50;
        public int SizeOffset = 50;

        public List<GameObject> CreateList(Transform parentTransform, IList<string> filenames)
        {
            List<GameObject> returnedList = new List<GameObject>();
            int offset = 0;
            int startOffset = CalculateStartOffset(filenames.Count);

            foreach (string filename in filenames)
            {
                GameObject item = Create(parentTransform, filename, startOffset - offset);
                returnedList.Add(item);

                offset += OffsetBetweenItems;
            }

            return returnedList;
        }

        int CalculateStartOffset(int filenamesCount)
        {
            return SizeOffset * filenamesCount / 2 - 15;
        }

        public GameObject Create(Transform parentTransform, string filename, int yPosition)
        {
            var item = Object.Instantiate(ItemPrefab);

            item.transform.SetParent(parentTransform);
            item.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            item.GetComponent<RectTransform>().localRotation = Quaternion.identity;
            item.GetComponent<RectTransform>().localPosition = new Vector3(0, yPosition, 0);
            item.GetComponentInChildren<Text>().text = filename;
            item.SetActive(true);

            return item;
        }

        public int GetYPositionFromCount(int filenamesCount)
        {
            return CalculateStartOffset(filenamesCount) - (filenamesCount * SizeOffset);  
        }
    }
}
