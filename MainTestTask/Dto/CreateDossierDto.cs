namespace MainTestTask.Dto
{
    public class CreateDossierDto
    {
        public int? ParentId { get; set; }
        public int OrderNumber { get; set; }
        public string SectionCode { get; set; }
        public string Name { get; set; }
    }
}
