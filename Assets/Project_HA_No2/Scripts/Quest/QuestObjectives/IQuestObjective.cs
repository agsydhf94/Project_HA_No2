
namespace HA
{
    public interface IQuestObjective
    {
        bool IsCompleted { get; }
        void Initialize(QuestInfoSO questInfo);
        void UpdateProgress(string id);
        string GetProgressDescription();
    }
}
