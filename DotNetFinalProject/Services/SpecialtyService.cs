using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetFinalProject.Models;

namespace DotNetFinalProject.Services
{
    public class SpecialtyService
    {
        private readonly ISpecialtyRepository _specialtyRepository;

        public SpecialtyService(ISpecialtyRepository specialtyRepository)
        {
            _specialtyRepository = specialtyRepository;
        }

        public async Task<List<Specialty>> GetSpecialties()
        {
            return await _specialtyRepository.GetAll();
        }

        public async Task<Specialty> GetSpecialty(long? id)
        {

            return await _specialtyRepository.GetOne(id);
        }

        public async Task CreateSpecialty(Specialty specialty)
        {
            _specialtyRepository.AddSpecialty(specialty);
            await _specialtyRepository.Save();
        }

        public async Task EditSpecialty(Specialty specialty)
        {
            _specialtyRepository.EditSpecialty(specialty);
            await _specialtyRepository.Save();
        }

        public async Task DeleteSpecialty(Specialty specialty)
        {
            _specialtyRepository.RemoveSpecialty(specialty);
            await _specialtyRepository.Save();
        }

        public bool SpecialtyExist(long? id)
        {
            return _specialtyRepository.Exists(id);
        }
    }
}