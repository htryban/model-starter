using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStarter
{
    public interface IFollowable
    {
        /// <summary>
        /// The IFollowable's position in the world 
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        /// The angle the IFollowable is facing, in radians 
        /// </summary>
        float Facing { get; set; }
    }

}
