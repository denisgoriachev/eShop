using eShop.Application.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Infrastructure.Persistance
{
    public class Checkpoint
    {
        public string Id { get; private set; }

        public ulong? Position { get; private set; }

        private Checkpoint()
        {
            Id = null!;
        }

        public Checkpoint(string id)
        {
            Id = id;
        }

        public void UpdatePosition(ulong newPosition)
        {
            if (newPosition < Position)
                throw new Exception($"New checkpoint position cannot be less then current position (new position = {newPosition}, current position = {Position}");

            Position = newPosition;
        }
    }
}
