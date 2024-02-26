using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nosqlconverter.MongoModels {
    public class MongoGame {

        public Guid _id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        private string _gameAlias;

        [Required]
        [MaxLength(100)]
        public string Key {
            get {
                if (string.IsNullOrEmpty(_gameAlias)) {
                    _gameAlias = Name.Replace(" ", "-").ToLower();
                }

                return _gameAlias;
            }
            set => _gameAlias = value;
        }

        [Column(TypeName = "decimal(18,4)")]
        public double Price { get; set; }

        public int Discount { get; set; }

        public int UnitInStock { get; set; }

        public Guid PublisherId { get; set; }
    }
}
