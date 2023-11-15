namespace MainTestTask.Dto
{
    public class DossierDto
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public int OrderNumber { get; set; }

        public string? SectionCode { get; set; }

        public string? Name { get; set; }

        public List<DossierDto> Children { get; set; }
    }
}
