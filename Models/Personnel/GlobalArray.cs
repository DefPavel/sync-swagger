using System.Collections.Generic;

namespace sync_swagger.Models.Personnel
{
    public class GlobalArray : ResponceSync
    {
        // Синхронизация для отделов и должностей
        public IEnumerable<Department> ArrayDepartments { get; set; }
        // Основная информация о человеке 
        public IEnumerable<Persons> ArrayPersons { get; set; }
        // Информация об отпусках
        public IEnumerable<Vacations> ArrayVacation { get; set; }
        // Информация о награждениях
        public IEnumerable<Rewarding> ArrayRewarding { get; set; }
        // Парсер сканов документов
        public IEnumerable<Documents> ArrayDocuments { get; set; }
        // Повыщение квалификации
        public IEnumerable<Qualification> ArrayQualification { get; set; }
        // Ученное звание
        public IEnumerable<UchZvanie> ArrayAcademicTitle { get; set; }
        // Служебные перемещения
        public IEnumerable<Move> ArrayMove { get; set; }
        // Научная степень
        public IEnumerable<ScientificDegree> ArrayDegrees { get; set; }
        // Парсер фотографий 3x4
        public IEnumerable<Image> ArrayImage { get; set; }
    }
}
