using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : SlotBase<QuestData>
{
    [SerializeField] TextMeshProUGUI txtName;
    [SerializeField] TextMeshProUGUI txtProgress;
    [SerializeField] TextMeshProUGUI txtStarReward;
    [SerializeField] Slider sProgress;
    [SerializeField] GameObject objCheck;
    [SerializeField] Animator animStar;
    float currentProgress;
    float questRequire;
    public override void InitData(QuestData data)
    {
        base.InitData(data);
        currentProgress = ProfileManager.Instance.playerData.questDataSave.GetCurrentProgress(data.questType);
        questRequire = data.questRequirebase;
        if (currentProgress > questRequire)
            currentProgress = questRequire;
        txtProgress.text = currentProgress.ToString() + "/" + questRequire;
        txtName.text = data.questName.ToString();
        txtStarReward.text = data.questStarEarn.ToString();
        sProgress.maxValue = questRequire; 
        sProgress.value = currentProgress;
        bool isClaimed = ProfileManager.Instance.playerData.questDataSave.IsClaimQuest(data.questType);
        btnChoose.interactable = currentProgress == questRequire && !isClaimed;
        objCheck.SetActive(currentProgress == questRequire && isClaimed);
        animStar.SetBool("CanClaim", btnChoose.interactable);
    }

    public override void OnChoose()
    {
        GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
        base.OnChoose();
        btnChoose.interactable = false;
        objCheck.SetActive(true);
        animStar.SetBool("CanClaim", false);
    }
}
