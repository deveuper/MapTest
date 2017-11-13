using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ComMap
{
    public class ComMapSystem : MonoBehaviour
    {
        //Target Player
        public GameObject TargetPlayer;
        //Arrow
        public GameObject TargetArrow;
        //using the target to get the value
        public GameObject TerrainToShow;

        public bool isTerrain = true;

        public GameObject TargetPoint;
        //using the cast map
        public GameObject MapToShow;
        //Get the Map Mask layer
        public GameObject MapMask;
        [HideInInspector]
        public MeshFilterInfo TargetInfo;
        [HideInInspector]
        public MeshFilterInfo MapInfo;
        [Space(1)]

        [Range(0.01f, 2f)]
        public float MapScaleValue = 1f;
        public Vector3 TerrainPlanePosition;
        private float ratioX;
        private float ratioZ;

        private float TerrainX;
        private float TerrainZ;

        private float MapX;
        private float MapZ;

        private float ScaleInit;

        private void Awake()
        {
            GetTargetHeightAndWidth();
            GetMapInfo();
        }
        // Use this for initialization
        void Start()
        {
            TerrainPlanePosition = TerrainToShow.transform.position;
        }
        Vector3 TargetPlayerPosition;
        // Update is called once per frame
        void Update()
        {
            TargetPlayerPosition = TargetPlayer.transform.position;
            PutPointIntoMap();
            MapScaleAndTerrianPos();
        }
        // Get the target width and height for cast ready
        private void GetTargetHeightAndWidth()
        {
            if (isTerrain)
            {
                GetTerrainOrPlaneInfo();
            }
            else
            {
                GetTargetInfo();
            }
        }

        private void GetTargetInfo()
        {
            TargetInfo.X = TargetPoint.transform.position.x;
            TargetInfo.Z = TargetPoint.transform.position.z;
            TerrainX = TargetInfo.X;
            TerrainZ = TargetInfo.Z;
        }

        private void GetTerrainOrPlaneInfo()
        {
            TargetInfo.X = TerrainToShow.GetComponent<MeshFilter>().mesh.bounds.size.x * TerrainToShow.transform.localScale.x;
            TargetInfo.Z = TerrainToShow.GetComponent<MeshFilter>().mesh.bounds.size.z * TerrainToShow.transform.localScale.z;
            TerrainX = TargetInfo.X / 2;
            //Debug.Log(TerrainX + " , " + TerrainZ);
            TerrainZ = TargetInfo.Z / 2;

        }
        // Get UI map image width and height value
        private void GetMapInfo()
        {
            MapInfo.X = MapToShow.GetComponent<RectTransform>().rect.width;
            MapInfo.Z = MapToShow.GetComponent<RectTransform>().rect.height;
            MapX = MapInfo.X / 2;
            MapZ = MapInfo.Z / 2;
        }
        // Calculate the ratio for the player position on terrain
        private void Calculate()
        {
            if (isTerrain)
            {
                ratioX = (TargetPlayerPosition.x - TerrainToShow.transform.position.x) / TerrainX;
                ratioZ = (TargetPlayerPosition.z - TerrainToShow.transform.position.z) / TerrainZ;
            }
            else
            {
                ratioX = (TargetPlayerPosition.x - TerrainToShow.transform.position.x) / Mathf.Abs(TargetPoint.transform.position.x - TerrainToShow.transform.position.x);
                ratioZ = (TargetPlayerPosition.z - TerrainToShow.transform.position.z) / Mathf.Abs(TargetPoint.transform.position.x - TerrainToShow.transform.position.x);
            }
            //Debug.Log(TargetPlayerPosition.x +","+ratioX + "," + TerrainX);
            //Debug.Log(ratioZ);
        }
        private void PutPointIntoMap()
        {
            Calculate();
            Debug.Log(MapScaleValue);
            float tempX = ratioX * MapX * MapScaleValue;
            float tempZ = ratioZ * MapZ * MapScaleValue;
            //Make the map move down,it sames like the playerArrow move up on the map
            MapToShow.GetComponent<RectTransform>().localPosition = new Vector3(-tempX, -tempZ, 0);
            //+ MapMask.GetComponent<RectTransform>().position;
            ArrowRotation();
        }
        // Arrow look at player's forward direction
        private void ArrowRotation()
        {
            TargetArrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, -TargetPlayer.transform.localEulerAngles.y - 180));
        }

        private void MapScaleAndTerrianPos()
        {
            MapToShow.GetComponent<RectTransform>().localScale = new Vector3(MapScaleValue, MapScaleValue, MapScaleValue);
            TerrainToShow.transform.position = TerrainPlanePosition.x != 0 ? TerrainPlanePosition : TerrainToShow.transform.position;
        }

        #region Next Version
        private void Bigger()
        {
            if (MapToShow.GetComponent<RectTransform>().localScale.x <= ScaleInit + 1)//make the biggest scale less than ScaleInit+1
            {
                MapToShow.GetComponent<RectTransform>().localScale.Set(MapToShow.GetComponent<RectTransform>().localScale.x + 0.2f,
                    MapToShow.GetComponent<RectTransform>().localScale.x + 0.2f, MapToShow.GetComponent<RectTransform>().localScale.x + 0.2f);
            }
        }
        private void Small()
        {
            if (MapToShow.GetComponent<RectTransform>().localScale.x >= ScaleInit - 1)//make the biggest scale more than ScaleInit+1
            {
                MapToShow.GetComponent<RectTransform>().localScale.Set(MapToShow.GetComponent<RectTransform>().localScale.x - 0.2f,
                MapToShow.GetComponent<RectTransform>().localScale.x - 0.2f, MapToShow.GetComponent<RectTransform>().localScale.x - 0.2f);
            }
        }
        #endregion
    }
    public struct MeshFilterInfo
    {
        public float X;
        public float Y;
        public float Z;
    }

}