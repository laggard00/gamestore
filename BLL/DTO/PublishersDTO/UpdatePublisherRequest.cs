using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Models;

namespace GameStore.BLL.DTO.PublishersDTO {
    public class UpdatePublisherRequest {

        public GameStore.DAL.Models.Publisher publisher { get; set; }
    }
}
