using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.Data;
using PaintIn3D;
using UnityEditor;
namespace Sculpturing.Levels {
    public class LevelInstance : MonoBehaviour
    {

        [SerializeField] private LevelObjects myObjects = null;
        public LevelObjects MyLevelObjects { get { return myObjects; } } 
        public void RecordData(LevelObjects data)
        {
            if (myObjects == null)
                myObjects = new LevelObjects();
            myObjects.Record(data);
        }


    }
}