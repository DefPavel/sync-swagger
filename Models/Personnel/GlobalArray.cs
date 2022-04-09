using System.Collections.Generic;

namespace sync_swagger.Models.Personnel
{
    public class GlobalArray : ResponceSync
    {
        // Синхронизация для отделов и должностей
        public IList<Department> ArrayDepartments { get; set; }
        // Основная информация о человеке 
        public IList<Persons> ArrayPersons { get; set; }
        // Информация об отпусках
        public IList<Vacations> ArrayVacation { get; set; }
        // Информация о награждениях
        public IList<Rewarding> ArrayRewarding { get; set; }
        // Парсер сканов документов
        public IList<Documents> ArrayDocuments { get; set; }
        // Повыщение квалификации
        public IList<Qualification> ArrayQualification { get; set; }
        // Ученное звание
        public IList<UchZvanie> ArrayAcademicTitle { get; set; }
        // Служебные перемещения
        public IList<Move> ArrayMove { get; set; }
        // Научная степень
        public IList<ScientificDegree> ArrayDegrees { get; set; }
        // Парсер фотографий 3x4
        public IList<Image> ArrayImage { get; set; }
    }
}
