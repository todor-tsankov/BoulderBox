namespace BoulderBox.Data.Models.Interfaces
{
    public interface IHaveImage
    {
        string ImageId { get; set; }

        Image Image { get; set; }
    }
}
