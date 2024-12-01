using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomComponent : MonoBehaviour
{
    private DungeonGenerator.Room roomData;
    private System.Action<DungeonGenerator.Room> onVisitCallback;

    public void Initialize(DungeonGenerator.Room room, System.Action<DungeonGenerator.Room> onVisit)
    {
        RoomId = room.Id;
        roomData = room;
        onVisitCallback = onVisit;
    }

    public int RoomId { get; set; } // Room ID to link this component to the Room instance

    private void OnMouseDown()
    {
        if (roomData != null && onVisitCallback != null)
        {
            onVisitCallback.Invoke(roomData);
        }
    }
}
