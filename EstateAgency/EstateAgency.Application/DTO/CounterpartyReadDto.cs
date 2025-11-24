    namespace EstateAgency.Application.DTO;

    /// <summary>
    /// DTO для отображения информации о контрагенте.
    /// Используется для передачи данных при запросах в API.
    /// </summary>
    public class CounterpartyReadDto
    {
        /// <summary>
        /// Уникальный идентификатор контрагента.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ФИО контрагента.
        /// </summary>
        public required string FullName { get; set; }

        /// <summary>
        /// Паспортный номер.
        /// </summary>
        public required string PassportNumber { get; set; }

        /// <summary>
        /// Телефонный номер.
        /// </summary>
        public required string Phone { get; set; }
    }