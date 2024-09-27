using System;
using System.Collections.Generic;

namespace Assets.Scripts.Saving
{
    public class QuestInfoSave : Save
    {
        public Quest[] quests;
        public List<Quest> completed;
        public List<Quest> inprogress;
        public List<Quest> failed;

        public QuestInfoSave(List<Quest> _completed, List<Quest> _inprogress, List<Quest> _failed)
        {
            ID = "QUESTSAVE";
            completed = _completed;
            inprogress = _inprogress;
            failed = _failed;
        }
        public QuestInfoSave(Quest[] quests)
        {
            ID = "QUESTSAVE";
            this.quests = quests;
        }
    }
}