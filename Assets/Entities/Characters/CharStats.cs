using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharStats : MonoBehaviour
{
    //Level and EXP
    private byte LVL = 0;
    private int EXPToNextLVL = -1;
    private int EXP = -1;
    private float EXPGrowthMod = 1f;

    //Health
    private int maxHP = -1;
    public int currHP = 0;
    private float HPGrowthMod = 1f;
    //Magic
    private int maxMP = -1;
    public int currMP = 0;
    private float MPGrowthMod = 1f;
    //Skill points
    public byte currSP = 0;

    //[Start] ----- Stats
    //Strength
    private short STR = -1;
    public short battleSTR = 0;
    private float STRGrowthMod = 1f;
    /// <summary>
    /// Phyiscal critical chance
    /// </summary>
    private float phiCrit = 0f;
    //Defense
    private short DEF = -1;
    public short battleDEF = 0;
    private float DEFGrowthMod = 1f;
    //Magic attack
    private short MAK = -1;
    public short battleMAK = 0;
    private float MAKGrowthMod = 1f;
    /// <summary>
    /// Magical critical chance
    /// </summary>
    private float magCrit = 0f;
    //Magic resistance
    private short MAR = -1;
    public short battleMAR = 0;
    private float MARGrowthMod = 1f;
    //Speed
    private short SPD = -1;
    public short battleSPD = 0;
    private float SPDGrowthMod = 1f;
    /// <summary>
    /// Modifier for cast time
    /// </summary>
    private float castTimeMod = 1f;
    //Dextarity
    private short DEX = -1;
    public short battleDEX = 0;
    private float DEXGrowthMod = 1f;
    //Accuracy mod
    private float ACC = 0f;
    public float battleACC = 0f;
    //Agility
    private short AGL = -1;
    public short battleAGL = 0;
    private float AGLGrowthMod = 1f;
    //Evasion mod
    private float EVA = 0f;
    public float battleEVA = 0f;
    //Movement
    private short MOV = -1;
    public short battleMOV = 0;
    //Range
    private sbyte RNG = -1;
    //[END] ----- Stats

    public CharStats(int EXP, float EXPGrowthMod, short HP, float HPGrowthMod, short MP, float MPGrowthMod, short STR, float STRGrowthMod, short DEF, float DEFGrowthMod, short MAK, float MAKGrowthMod, short MAR, float MARGrowthMod,
        short SPD, float SPDGrowthMod, short DEX, float DEXGrowthMod, short AGL, float AGLGrowthMod, short MOV, sbyte RNG)
    {
        this.EXPGrowthMod = EXPGrowthMod;
        this.maxHP = HP;
        this.HPGrowthMod = HPGrowthMod;
        this.maxMP = MP;
        this.MPGrowthMod = MPGrowthMod;
        this.STR = STR;
        this.STRGrowthMod = STRGrowthMod;
        this.DEF = DEF;
        this.DEFGrowthMod = DEFGrowthMod;
        this.MAK = MAK;
        this.MAKGrowthMod = MAKGrowthMod;
        this.MAR = MAR;
        this.MARGrowthMod = MARGrowthMod;
        this.SPD = SPD;
        this.SPDGrowthMod = SPDGrowthMod;
        this.DEX = DEX;
        this.DEXGrowthMod = DEXGrowthMod;
        this.AGL = AGL;
        this.AGLGrowthMod = AGLGrowthMod;
        this.MOV = MOV;
        this.RNG = RNG;

        increaseEXP(EXP);
    }

    public void increaseEXP(int exp)
    {
        EXP += exp;
        EXPToNextLVL -= exp;

        if (EXPToNextLVL <= 0)
        {
            lvlUp();
        }
    }

    private void lvlUp()
    {
        while (EXP < EXPToNextLVL)
        {
            LVL++;
            EXPToNextLVL = Mathf.RoundToInt((LVL * 0.8f) * ((5000 * EXPGrowthMod) * (LVL * 0.3f)));
            maxHP = (int)(LVL * (106f * (HPGrowthMod * 1.09f)) + (1102 * (HPGrowthMod * 0.68f)));
            maxMP = (int)(LVL * (1.472f * MPGrowthMod) + (325f * (MPGrowthMod * 0.92f)));
            STR = (short) (LVL * (7 * STRGrowthMod) + (50 * (STRGrowthMod * 0.5f)));
            DEF = (short)(LVL * (6 * DEFGrowthMod) + (50 * (DEFGrowthMod * 0.5f)));
            MAK = (short)(LVL * (4.6f * MAKGrowthMod) + (50 * (MAKGrowthMod * 0.5f)));
            MAR = (short)(LVL * (4 * MARGrowthMod) + (50 * (MARGrowthMod * 0.5f)));
            SPD = (short)(LVL * (1.76f * SPDGrowthMod) + (12 * (SPDGrowthMod * 0.6f)));
            DEX = (short)(LVL * (0.8f * DEXGrowthMod) + (8.67f * (DEXGrowthMod * 0.6f)));
            AGL = (short)(LVL * (0.72f * AGLGrowthMod) + (8.9f * (AGLGrowthMod * 0.62f)));
        }
    }

    public void resetBattleStats()
    {
        battleSTR = STR;
        battleDEF = DEF;
        battleMAK = MAK;
        battleMAR = MAR;
        battleSPD = SPD;
        battleDEX = DEX;
        battleACC = ACC;
        battleAGL = AGL;
        battleEVA = EVA;
        battleMOV = MOV;
    }
}
