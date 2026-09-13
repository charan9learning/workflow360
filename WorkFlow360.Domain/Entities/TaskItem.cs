using WorkFlow360.Domain.Enums;

namespace WorkFlow360.Domain.Entities
{
    public sealed class TaskItem
    {
        public Guid Id { get; private set; }

        public Guid ProjectId { get; private set; }

        public string Title { get; private set; } = string.Empty;

        public string? Description { get; private set; }

        public TaskItemStatus Status { get; private set; }

        public Guid? AssignedUserId { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }

        public DateTime? UpdatedAtUtc { get; private set; }

        private TaskItem()
        {
        }

        public TaskItem(
            Guid projectId,
            string title,
            string? description)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentException("Project is required.");

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.");

            Id = Guid.NewGuid();
            ProjectId = projectId;
            Title = title.Trim();
            Description = description;

            Status = TaskItemStatus.ToDo;

            CreatedAtUtc = DateTime.UtcNow;
        }

        public void AssignTo(Guid userId)
        {
            AssignedUserId = userId;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void ChangeStatus(TaskItemStatus status)
        {
            Status = status;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
