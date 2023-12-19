using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyQuestSheet : SheetBase<QuestData>
{
    public override void LoadData(List<QuestData> datas)
    {
        base.LoadData(datas);
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].questType==QuestType.Ranking) { listSlots[i].gameObject.SetActive(false);break; }

        }
    }
}
