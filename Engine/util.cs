using Box2DX;
using Box2DX.Dynamics;
using Box2DX.Common;
using Box2DX.Collision;

using System.Linq;
using System.Collections.Generic;

public class Vector2
{
    public Vector2(int nx, int ny){ x = nx; y = ny; }
    public Vector2(float nx, float ny)
    {
        x = nx;
        y = ny;        
    }

    public float x, y;

    #region Operators

    public static Vector2 operator +(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x + v2.x, v1.y + v2.y);
    }

    public static Vector2 operator -(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x - v2.x, v1.y - v2.y);
    }

    public static Vector2 operator *(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x * v2.x, v1.y * v2.y);
    }

    public static Vector2 operator /(Vector2 v1, float v2)
    {
        return new Vector2(v1.x / v2, v1.y / v2);
    }

    public static Vector2 operator *(Vector2 v1, float v2)
    {
        return new Vector2(v1.x * v2, v1.y * v2);
    }

    public static Vector2 operator -(Vector2 v1, float v2)
    {
        return new Vector2(v1.x - v2, v1.y - v2);
    }

    public static Vector2 operator +(Vector2 v1, float v2)
    {
        return new Vector2(v1.x + v2, v1.y + v2);
    }

    public static Vector2 operator +(Vector2 v1, Angle2D v2)
    {
        return new Vector2(v1.x + (float)v2.x, v1.y + (float)v2.y);
    }

    public static Vector2 operator -(Vector2 v1, Angle2D v2)
    {
        return new Vector2(v1.x - (float)v2.x, v1.y - (float)v2.y);
    }

    public static Vector2 operator /(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x / v2.x, v1.y / v2.y);
    }

    public static bool operator ==(Vector2 v1, Vector2 v2)
    {
        return (v1.x == v2.x && v1.y == v2.y);
    }

    public static bool operator !=(Vector2 v1, Vector2 v2)
    {
        return (v1.x != v2.x || v1.y != v2.y);
    }

    public static bool operator >(Vector2 v1, Vector2 v2)
    {
        return (v1.x >v2.x && v1.y > v2.y);
    }

    public static bool operator <(Vector2 v1, Vector2 v2)
    {
        return (v1.x < v2.x && v1.y < v2.y);
    }

    public static bool operator <=(Vector2 v1, Vector2 v2)
    {
        return (v1.x <= v2.x || v1.y <= v2.y);
    }

    public static bool operator >=(Vector2 v1, Vector2 v2)
    {
        return (v1.x >= v2.x || v1.y >= v2.y);
    }

#endregion

    public static Vector2 Zero
    {
        get { return new Vector2(0, 0); }
    }

    public override int GetHashCode()
    {
        return (int)x ^ (int)y;
    }

    public Vec2 ToVec2()
    {
        return new Vec2((float)this.x, (float)this.y);
    }

    /*public bool WithinAABB(ref AABB bounds)
    {
        return (this.x <= bounds.UpperBound.X && this.x >= bounds.LowerBound.X) && (this.y <= bounds.UpperBound.Y && this.y >= bounds.LowerBound.Y);
    }*/

    public void Floor()
    {
        x = (float)System.Math.Floor(this.x);
        y = (float)System.Math.Floor(this.y);
    }

    public Vector2 Floored()
    {
        return new Vector2((float)System.Math.Floor(this.x), (float)System.Math.Floor(this.y));
    }

    public bool Within(Vector2 min, Vector2 max)
    {
        return (this > min && this < max);
    }

    public bool WithinRadius(Vector2 min, Vector2 max, float radius)
    {
        return (this > min - radius && this < max + radius);
    }

    public Vector2 Squared()
    {
        return this * this;
    }

    public Vector2 Clamp(int high, int low)
    {
        float nx = this.x, ny = this.y;

        if(nx > high)
            nx = high;
        if(nx < low)
            nx = low;

        if(ny > high)
            ny = high;
        if(ny < low)
            ny = low;

        return new Vector2(nx, ny);
    }

    public Vector2 Normalize()
    {
        float mag = this.Length();

        float Vx, Vy;

        Vx = x / mag;
        Vy = y / mag;

        return new Vector2(Vx, Vy);
    }

    public float Length()
    {
        return Math.Sqrt( ((x * x) + (y * y)) );
    }

    public double DistTo(Vector2 input)
    {
        float a, b, c;
        a = input.x - this.x;
        b = input.y - this.y;

        c = ( (a * a) + (b * b) );

        return (c * (c * 0.5));
    }

    public override string ToString()
    {
        return string.Format("Vector({0}, {1})", this.x, this.y);
    }
}

public class AABB
{
    public float x, y;
    public float width, height;

    public AABB(double x, double y, double width, double height)
    {

    }

    public static AABB Zero()
    {
        return new AABB(0, 0, 0, 0);
    }

    public bool intersectsRadius(Vector2 position, float radius)
    {
        Vector2[] points = new Vector2[] {
            new Vector2(x, y),
            new Vector2(x + width, y),
            new Vector2(x, y+ height),
            new Vector2(x + width, y + height)
        };

        /*foreach (Vector2 point in points)
        {
            if (position.DistTo(point) < radius)
            {
                return true;
            }
        }*/

        if(position.WithinRadius(points[0], points[3], radius))
        {
            return true;
        }

        return false;
    }
}

public class Angle2D
{
    //Apparently Rotation Matrices work in Radians, and not degrees.

    //Before this class, i was attempting to use degrees for my rotations
    //resulting in false angles being presented.

    //This class will allow you to go to and from either Degrees or Radians to a Rotation Matrix.

    public float x, y;// angle;

    private const double degMult = System.Math.PI / 180;
    private const double radMult = 180 / System.Math.PI;

    #region Constructors
    /// <summary>
    /// Creates a new Angle2D Matrix class
    /// </summary>
    /// <param name="Degrees">Angle in Degrees</param>
    public Angle2D(int Degrees)
    {
        x = 0; y = 1;

        Set(Degrees);
    }

    /// <summary>
    /// Creates a new Angle2D Matrix class
    /// </summary>
    /// <param name="Radians">Angle in Radians</param>
    public Angle2D(float Radians)
    {
        x = 0; y = 1;

        Set(Radians);
    }

    /// <summary>
    /// Creates a new Angle2D Matrix class
    /// </summary>
    /// <param name="xMatrix">Matrix X Value</param>
    /// <param name="yMatrix">Matrix Y Value</param>
    public Angle2D(float xMatrix, float yMatrix)
    {
        x = xMatrix; y = yMatrix;
    }
    #endregion
    
    #region Operators

    public void Set(int Degrees)
    {
        double angle = (( Degrees * System.Math.PI) / 180);

        x = (float)(0 * System.Math.Cos(angle) - -1 * System.Math.Sin(angle));
        y = (float)(0 * System.Math.Sin(angle) + -1 * System.Math.Cos(angle));
    }

    public void Set(float Radians)
    {
        double angle = Radians * System.Math.PI;

        x = (float)(0 * System.Math.Cos(angle) - -1 * System.Math.Sin(angle));
        y = (float)(0 * System.Math.Sin(angle) + -1 * System.Math.Cos(angle));
    }

    public void Add(int Degrees)
    {
        double PreX = x;
        double angle = ((Degrees * System.Math.PI) / 180);

        x = (float)(x * System.Math.Cos(angle) - y * System.Math.Sin(angle));
        y = (float)(PreX * System.Math.Sin(angle) + y * System.Math.Cos(angle));
    }

    public void Add(float Radians)
    {
        double PreX = x;
        double angle = Radians * System.Math.PI;

        x = (float)(x * System.Math.Cos(angle) - y * System.Math.Sin(angle));
        y = (float)(PreX * System.Math.Sin(angle) + y * System.Math.Cos(angle));
    }

    public static Angle2D operator *(Angle2D v1, float v2)
    {
        return new Angle2D(v1.x * v2, v1.y * v2);
    }

    public static Angle2D operator +(Angle2D v1, Angle2D v2)
    {
        return new Angle2D(v1.x + v2.x, v1.y + v2.y);
    }

    public Angle2D Right()
    {
        Angle2D ang = new Angle2D(this.x, this.y);        
        ang.Add(90);
        return ang;
    }

    public double GetRadians()
    {
        return System.Math.Atan2(y, x);
    }

    #endregion

    #region TypeCasts

    public Vector2 ToVector2() { return new Vector2((float)this.x, (float)this.y); }

    #endregion
}

public class Ray
{
    public Vector2 mStartPosition, mEndPosition; //Start and end, for distance purposes
    public Vector2 mDirection;

    ENT_TYPE mCollType = ENT_TYPE.ENT_GENERIC; //All types

    float mStepAmount = 0.1f;
    int mFarZ = 2048; //Minimum and Maximum distance
    int mDetectRadius = 1;
    public bool mCollided = false;

    public object Cast(Vector2 startPos, Vector2 dir, ENT_TYPE collisionType)
    {
        mStartPosition = startPos;
        mDirection = dir;
        mEndPosition = startPos + (dir * mFarZ); //Give us our farZ coord (startpoint + (direction * scalar))

        float lerpMult = 0;
        while (lerpMult < 1)
        {
            lerpMult += 0.01f;

            Vector2 lerpPos = (mEndPosition - mStartPosition) * lerpMult;

            switch (collisionType)
            {
                case ENT_TYPE.ENT_GEOM:
                    mCollided = TestGeom(lerpPos);
                    break;

                default:
                    mCollided = TestEntGeneric();
                    break;
            }
        }

        return null;
    }

    private bool TestGeom(Vector2 pos)
    {
        List<Engine.World.base_geom_entity> geom = Engine.Engine.mWorld.mGeometry;

        //return (geom.Find(i => i.mBounds.ContainsVector2(pos)) != null);

        return false;
    }

    private bool TestEntGeneric()
    {
        return false;
    }
}

enum PhysBoundType
{
    BOUNDING_BOX = 1,
    BOUNDING_CIRCLE,   
}

public class UTIL_WORLD
{
    public static bool CollidingWithWorld(Vector2 pos, int radius)
    {
        Engine.World.cWorld mWorld = Engine.Engine.mWorld;

        //Get all geometry in a 5 block radius
        List<Engine.World.base_geom_entity> geom = mWorld.mGeometry.FindAll(i => i.mPosition.DistTo(pos) < radius);

        if (geom.Count > 0)
            return true;

        return false;
    }
}

public enum ENT_TYPE
{
    ENT_PLAYER = 0,

    ENT_STATIC = 1,
    ENT_MOVEABLE,
    ENT_USEABLE,
    ENT_TRIGGER,
    ENT_SMART,
    ENT_GENERIC,

    ENT_GEOM =10,
    ENT_MOVEABLE_GEOM,
    ENT_TRIGGER_GEOM,    
}

public enum ENGINE_CONST
{
    C_ERROR = 1,
    C_WARNING,

    C_MSG = 10,
}

public enum INPUT_BITS
{
    //Name = bit
    IN_UP = 0x01,
    IN_DOWN = 0x02,
    IN_LEFT = 0x04,
    IN_RIGHT = 0x08,
    IN_ATTACK1 = 0x10,
    IN_ATTACK2 = 0x20,
    IN_USE = 0x40,
    IN_INVENTORY = 0x80,
    IN_CHARSHEET = 0x100,
    IN_MOUSE = 0x200,
    IN_SPRINT = 0x400,
}

public enum ROOM_TYPES
{
    TYPE_BRIMSTONE = 0,
    TYPE_WOOD,
    TYPE_COBBLESTONE,
    TYPE_TILE,
}

public class math
{
    public static int Clamp(int i, int max, int min)
    {
        if (i > max)
            i = max;

        if (i < min)
            i = min;

        return i;
    }

    public static double Clamp(double i, double max, double min)
    {
        if (i > max)
            i = max;

        if (i < min)
            i = min;

        return i;
    }

    public static double TrimTenth(double input)
    {
        return ((int)(input / 0.01) * 0.01);
    }
}