using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.Data
{

    public struct Tags
    {
        public const string Spawn = "Spawn";
        public const string Statue = "Statue";
        public const string Stone = "Stone";
    }
    public struct StageByName
    {
        public const string Sculpturing = "Sculpturing";
        public const string Polishing = "Polishing";
        public const string Painting = "Painting";
        public const string Decorating = "Decorating";
    }
    public struct SoundByName
    {
        public const string StoneHit = "Sculpturing";
        public const string StonePolish = "Polishing";
        public const string Spray = "Painting";
    }
    public struct SourceByName
    {
        public const string MusicSource = "MusicSource";
        public const string EffectsSource = "EffectsSource";
    }
    public struct MaterialTypes
    {
        public const string Statue = "Statue";
        public const string MainStone = "Stone";
        public const string StonePieces = "Pieces";
        public const string StoneShards = "Shards";
    }
}