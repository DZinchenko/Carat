using Carat.Data.DTOs;

namespace Carat.Data.Repositories
{
    public interface IReportDTORepository
    {
        FinalTeachersReportDTO getFinalTeachersReportDTO(string educType, string educForm, uint course, uint semestr, string educlevel);
        FinalSubjectsReportDTO getFinalSubjectsReportDTO(string educType, string educForm, uint course, uint semestr, string educlevel);
    }
}
