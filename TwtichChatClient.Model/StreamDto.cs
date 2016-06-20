using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwtichChatClient.Model
{
    public class Stream : IEquatable<string>
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ChannelName { get; set; }
        public static explicit operator StreamDto(Stream stream)
        {
            if (stream == null)
                return null;
            return new StreamDto
            {
                CreatedAt = stream.CreatedAt,
                Title = stream.Title,
                Id = stream.Id
            };
        }

        public bool Equals(string other)
        {
            return ChannelName == other;
        }

        public override int GetHashCode()
        {
            return ChannelName.GetHashCode();
        }
    }
    [Table("Streams")]
    public class StreamDto
    {
        [Key]
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

        public override bool Equals(object other)
        {
            var dto = other as StreamDto;
            return Id == dto?.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}