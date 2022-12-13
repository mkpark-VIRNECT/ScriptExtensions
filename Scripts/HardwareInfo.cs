using System;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[CreateAssetMenu()]
public class HardwareInfo : ScriptableObject
{
    [Serializable]
    public struct HardwareSpec
    {
        public int processCount;
        public int processFrequency;

        public int systemMemory;

        public int graphicsMemorySize;
        public int graphicsShaderLevel;
        public int maxTextureSize;

        [Button]
        public void InitFromThisDevice ()
        {
            processCount = SystemInfo.processorCount;
            processFrequency = SystemInfo.processorFrequency;

            systemMemory = SystemInfo.systemMemorySize;

            graphicsMemorySize = SystemInfo.graphicsMemorySize;
            graphicsShaderLevel = SystemInfo.graphicsShaderLevel;
            maxTextureSize = SystemInfo.maxTextureSize;
        }

        public static float GetScore (HardwareSpec targetSpec, HardwareSpec referenceSpec)
        {
            float score = 100;
            score *= ( float ) targetSpec.processCount / referenceSpec.processCount;
            score *= ( float ) targetSpec.processFrequency / referenceSpec.processFrequency;

            score *= ( float ) targetSpec.systemMemory / referenceSpec.systemMemory;

            score *= ( float ) targetSpec.graphicsMemorySize / referenceSpec.graphicsMemorySize;
            score *= ( float ) targetSpec.graphicsShaderLevel / referenceSpec.graphicsShaderLevel;
            score *= ( float ) targetSpec.maxTextureSize / referenceSpec.maxTextureSize;

            return score;
        }
    }
    const string hardwareInfoRscPath = "HardwareInfo";
    static HardwareInfo _instance;
    static public HardwareInfo Instance { get { return _instance ?? ( _instance = Resources.Load(hardwareInfoRscPath) as HardwareInfo ); } }

    public HardwareSpec referenceSpec;

    public float GetScoreThisDevice ()
    {
        var thisDeviceSpec = new HardwareSpec();
        thisDeviceSpec.InitFromThisDevice();
        return HardwareSpec.GetScore(thisDeviceSpec, referenceSpec);
    }

    public int GetTierThisDevice ()
    {
        var score = GetScoreThisDevice();
        if (score >= 75)
            return 2;
        else if (score >= 50)
            return 1;
        else if (score >= 25)
            return 0;
        return 0;
    }
    [Button]
    void LogScore ()
    {
        Debug.Log($"Score = {Instance.GetScoreThisDevice()}");
    }

    [Button]
    void LogHardWare ()
    {
        string hardwareSpec = "";

        hardwareSpec += ( "-------Processor-------" ) + Environment.NewLine;
        hardwareSpec += ( SystemInfo.processorCount ) + Environment.NewLine;
        hardwareSpec += ( SystemInfo.processorFrequency ) + Environment.NewLine;
        hardwareSpec += ( SystemInfo.processorType ) + Environment.NewLine;
        hardwareSpec += ( "-------Memory-------" ) + Environment.NewLine;
        hardwareSpec += ( SystemInfo.systemMemorySize ) + Environment.NewLine;
        hardwareSpec += ( "-------Graphics-------" ) + Environment.NewLine;
        hardwareSpec += ( SystemInfo.graphicsMemorySize ) + Environment.NewLine;
        hardwareSpec += ( SystemInfo.graphicsShaderLevel ) + Environment.NewLine;
        hardwareSpec += ( SystemInfo.graphicsDeviceName ) + Environment.NewLine;
        hardwareSpec += ( SystemInfo.graphicsDeviceVersion ) + Environment.NewLine;
        Debug.Log(hardwareSpec);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void CheckFirstRun ()
    {
        if (!PlayerPrefs.HasKey("IsFirstRun"))
        {
            Instance.LogScore();
            PlayerPrefs.SetInt("IsFirstRun", 1);
            //LocalPrefs.Graphic pref = LocalPrefs.Get<LocalPrefs.Graphic>();
            //if (pref != null)
            //{
            //    var tier = Instance.GetTierThisDevice();
            //    pref.qualityLevel = tier;
            //    pref.resolution = tier == 0 ? tier : ( tier + 1 );
            //    pref.Save();
            //    G.GraphicManager.SetGraphicSetting(pref);
            //}
        }
    }

    [Button]
    void RemoveFirstRunKey ()
    {
        PlayerPrefs.DeleteKey("IsFirstRun");
    }
}
