using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDriveMotor_MotorGangster : BuffBase
{
    void Awake()
    {
        buffName = "BuffDriveMotor_MotorGangster";
        buffNameCN = "飞驰疾行";
        durationTurn = 10;
        buffType = BuffType.Buff;
        description = "摩托暴徒的行动力+1";
        canBeLayed = false;
        buffType = BuffType.Buff;
        buffImgType = BuffImgType.Action;
    }
    public override void OnAdd()
    {
        MotorGangster motorGangster = chessBase as MotorGangster;
        motorGangster.mobility += 1;
    }
    public override int OnHurt(int damage, ChessBase attacker, DamageType damageType = DamageType.Null)
    {
        BuffManager.instance.DeleteBuff("BuffDriveMotor_MotorGangster", chessBase);
        return damage;
    }
    public override void OnRemove()
    {
        MotorGangster motorGangster = chessBase as MotorGangster;
        motorGangster.mobility -= 1;
    }
}
