namespace Homework.Entities.ViewModel.Category
{
    public abstract class CategoryBaseModel
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
    }
}
