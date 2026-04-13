using System;
using UnityEngine;

public class Mazecontroller : MonoBehaviour
{

    [SerializeField] private float Difficulty;
    [SerializeField] private float MazeSizeWidth;   
    [SerializeField] private float MazeSizeHeight;

    [SerializeField] private GameObject MazeObject;
    [SerializeField] private float MazeObjectSize;




    void Awake()    
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Generate Maze")]
    public void GenerateMaze()
    {
        // 기존 로직 수정: 짝수일 때 에러가 발생해야 합니다.
        if(MazeSizeHeight % 2 == 0 || MazeSizeWidth % 2 == 0)
        {
            Debug.LogError("미로는 홀수로 설정해야 합니다.");
            return;
        }

        if(MazeSizeHeight < 5 || MazeSizeWidth < 5)
        {
            Debug.LogError("미로의 크기는 최소 5x5 이상이어야 합니다.");
            return;
        }

        int width = (int)MazeSizeWidth;
        int height = (int)MazeSizeHeight;
        int[,] maze = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1;
            }
        }

        for (int x = 1; x < width; x += 2)
        {
            for (int y = 1; y < height; y += 2)
            {
                maze[x, y] = 0; // 현재 칸을 길로 만듦

                bool canGoRight = (x + 2 < width);
                bool canGoUp = (y + 2 < height);

                if (canGoRight && canGoUp)
                {
                    // 오른쪽이나 위쪽 중 무작위로 하나를 선택해 길을 뚫습니다.
                    if (UnityEngine.Random.Range(0, 2) == 0)
                        maze[x + 1, y] = 0;
                    else
                        maze[x, y + 1] = 0;
                }
                else if (canGoRight)
                {
                    maze[x + 1, y] = 0;
                }
                else if (canGoUp)
                {
                    maze[x, y + 1] = 0;
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1) // 1인 위치에만 벽을 세웁니다.
                {
                    Vector3 pos = new Vector3(x * MazeObjectSize, y * MazeObjectSize, 0);
                    Instantiate(MazeObject, pos, Quaternion.identity, this.transform);
                }
            }
        }
    }

    public void RemoveMaze()
    {
        
    }
}
