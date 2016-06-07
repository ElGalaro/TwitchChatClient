using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwtichChatClient.Model
{
    public class Stream
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }

        public static explicit operator StreamDto(Stream stream)
        {
            return new StreamDto
            {
                CreatedAt = stream.CreatedAt,
                Title = stream.Title,
                Id = stream.Id
            };
        }
    }
    [Table("Streams")]
    public class StreamDto : IEquatable<StreamDto>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }

        public static explicit operator Stream(StreamDto dto)
        {
            return new Stream
            {
                CreatedAt = dto.CreatedAt,
                Title = dto.Title,
                Id = dto.Id
            };
        }

        public bool Equals(StreamDto other)
        {
            return Id == other.Id;
        }
    }
}