 # Booking And Manager Hotel
 ## 1. Báo cáo gồm:
 - web api + client
 - báo cáo bản mềm: https://docs.google.com/document/d/1sdVz0N3oU6t2AWFgL1TDJhTqsnuuOe0G/edit?usp=sharing&ouid=105238185890006122819&rtpof=true&sd=true
 - sql: https://drive.google.com/file/d/1bRqNFeMc8c6SHrSD3Gw_ta3R0Yce-FRx/view?usp=sharing
## 2. Hướng Dẫn Sử Dụng
- Clone đường dẫn github vào visual studio microsoft
- Sau khi hoàn thành, chọn web client ở solution explorer, chọn `wwwroot` -> `img`
- Click chuột file vào `new_pic`, chọn properties để copy đường dẫn full path
- Chọn `web api` -> `controller` -> `admincontroller`, ở dòng 20 thay đường dẫn phía trên vào
- Chọn cửa sổ solution, chuột phải vào solution `QuanLyKhachSanAPI`, chọn properties
- Tại cửa sổ này, chọn multiple starup projects, action bật start cả 2 projects ở phía dưới
- Mở sql server lấy tên máy chủ, sau đó chọn `model` ở api, chọn file `context` để thay đổi `servername` ở dòng 38
## 3. Hình ảnh chương trình:
![image](https://github.com/buck1704/BookingAndManagerHotelWebAPI/assets/132087690/eb4252f7-07d6-4963-9266-0c2a51171c73)
<div align="center">Giao Diện Đăng Nhập</div>
![image](https://github.com/buck1704/BookingAndManagerHotelWebAPI/assets/132087690/d33fd20c-898f-48c6-b839-f740d4bea3a7)
<div align="center">Giao Diện Trang Chủ</div>
![image](https://github.com/buck1704/BookingAndManagerHotelWebAPI/assets/132087690/03a1b48e-87ea-4372-bb7d-5947b45d5625)
<div align="center">Giao Diện CheckIn</div>
![image](https://github.com/buck1704/BookingAndManagerHotelWebAPI/assets/132087690/832c4004-3868-4744-9fa5-ddbc4aa971c6)
<div align="center">Giao Diện Kiểm Tra Phòng</div>
![image](https://github.com/buck1704/BookingAndManagerHotelWebAPI/assets/132087690/533ebff1-4a61-4bb2-b01f-5a55b8d6c7dd)
<div align="center">Giao Diện Quản Lý</div>
