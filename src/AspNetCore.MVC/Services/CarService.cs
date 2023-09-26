using AspNetCore.MVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCore.MVC.Services
{
    public class CarService
    {
        private readonly ETransportationSystemContext _context;

        public CarService()
        {
            _context = new ETransportationSystemContext();
        }

        public void Delete(int id)
        {

            //Car CarToDelete = new Car() { Id = id };
            //_context.Entry(CarToDelete).State = EntityState.Deleted;
            //_context.SaveChanges();

            Car _car = _context.Cars.FirstOrDefault(c => c.Id == id);
            if (_car != null)
            {
                _context.Cars.Remove(_car);
                _context.SaveChanges();

            }

        }

        public Car Get(int id)
        {
            // AsNoTracking() để chỉ định rằng các thực thể trả về từ truy vấn sẽ không được theo dõi (tracking).
            // Khi sử dụng phương thức này, Entity Framework Core sẽ không theo dõi sự thay đổi của các thực thể được trả về và không tự động cập nhật cơ sở dữ liệu khi lưu các thay đổi.
            // Khi sử dụng .AsNoTracking(), truy vấn trả về các thực thể sẽ có hiệu suất tốt hơn vì không cần theo dõi các thay đổi và xử lý việc cập nhật
            //Lưu ý rằng khi sử dụng .AsNoTracking(), các thực thể không được theo dõi không được gắn kết với DbContext.
            // Điều này có nghĩa là nếu bạn muốn thực hiện các thay đổi trên các thực thể và lưu chúng vào cơ sở dữ liệu,
            // bạn cần gắn kết các thực thể đó với DbContext bằng cách sử dụng phương thức Attach() hoặc Update().

            return _context.Cars.AsNoTracking()
                .Include(c => c.AccountSupplier).AsNoTracking()
                .Include(c => c.Model.Brand).AsNoTracking()
                .Include(c => c.CarImages).AsNoTracking()
                .Include(c => c.CarFeatures).ThenInclude(cf => cf.Feature).AsNoTracking()
                .Include(c => c.Books).ThenInclude(rv => rv.Review).AsNoTracking()
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Car> GetAll(int page, int size)
        {
            return _context.Cars.Skip((page - 1) * size).Take(size)
                .Include(c => c.Model.Brand)
                .Include(c => c.CarImages)
                .Include(c => c.CarFeatures).ThenInclude(cf => cf.Feature);
        }

        public void Update(Car Car)
        {
            // cach 1
            var _car = _context.Cars.FirstOrDefault(c => c.Id == Car.Id);
            _car.Fuel = Car.Fuel;
            _car.Description = Car.Description;
            _car.LicensePlates = Car.LicensePlates;
            _car.Transmission = Car.Transmission;
            _car.Price = Car.Price;
            _car.YearOfManufacture = Car.YearOfManufacture;
            // nên update   _car.ModelId = Car.ModelId; 
            _car.ModelId = Car.ModelId;
            // nếu update    _car.Model = Car.Model; thì phải có  _context.Models.Attach(Car.Model);
            _context.Models.Attach(Car.Model);
            _car.Model = Car.Model;
            _context.SaveChanges();

            //_context.Entry(Car).Property(x => x.ModelId).IsModified = false;
            //_context.Entry(Car).Reference(x => x.AccountSupplier).IsModified = false;
            //var entry = _context.Entry(Car);
            //foreach (var reference in entry.References)
            //{
            //    reference.IsModified = false;
            //}

            // cach 2 = neu propertes nao khong truyen ve thi null
            //_context.Attach(Car).State = EntityState.Modified;

            //_context.SaveChanges();

            //==================================================================================================
            //var _carImages = new List<CarImage>{
            //          new CarImage
            //         {
            //             Id = 755,
            //             Image = "aaafff"
            //         },
            //           new CarImage
            //         {
            //             Id = 0,
            //             Image = "bbbb"
            //         }
            //     };

            // nếu không có Include thì list cũ sẽ không bị xóa và data trong líst mới sẽ cộng dòn vào list cũ 
            // nếu có Include thì list cũ sẽ bị xóa và chỉ data trong líst mới sẽ add vào 
            // nếu có 1 data trong  list mới  có id giống id của 1 data trong list cũ thì data trong list cũ sẽ update theo data trong list mới

            //var car = _context.Cars.Include(c => c.CarImages).FirstOrDefault(c => c.Id == 124);

            // cách 1 pro 
            //car.CarImages = _carImages;

            // cách 2 giống bên spring hơi  phèn 
            //car.CarImages.Clear();
            //car.CarImages.AddRange(_carImages);
            //_context.SaveChanges();
        }

    }
}