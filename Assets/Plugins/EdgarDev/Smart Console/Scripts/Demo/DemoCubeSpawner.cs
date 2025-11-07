using UnityEngine;

#pragma warning disable IDE0051

namespace ED.SC.Demo
{
	public class DemoCubeSpawner : MonoBehaviour
	{
		[SerializeField] private Transform[] m_Spots;

		private GameObject[] m_Cubes;

		private void Start()
		{
			m_Cubes = new GameObject[m_Spots.Length];
		}

		private GameObject InstantiateCube(int spotIndex)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			gameObject.transform.position = m_Spots[spotIndex].position;
			gameObject.transform.rotation = Quaternion.identity;

			DemoCubeSpinner demoCubeSpinner = gameObject.AddComponent<DemoCubeSpinner>();
			demoCubeSpinner.SpinInterpolation = 1.0f;

			Debug.Log($"Spawned a cube.");

			return gameObject;
		}

		[Command("spawn_cube", "spawn a cube at available spot", MonoTargetType.Single)]
		private void Spawn()
		{
			int index = -1;

			for (int i = 0; i < m_Cubes.Length && index == -1; i++)
			{
				if (!m_Cubes[i])
				{
					// this spot is available
					index = i;
				}
			}

			if (index == -1)
			{
				SmartConsole.LogError($"Each spot is busy.");

				return;
			}

			m_Cubes[index] = InstantiateCube(index);
		}

		[Command("spawn_cube_at_index", "spawn a cube at spot index x", MonoTargetType.Single)]
		private void SpawnAtIndex(int index)
		{
			if (index < 0 || index >= m_Spots.Length)
			{
				SmartConsole.LogError($"Index {index} is out of range.");

				return;
			}

			if (m_Cubes[index])
			{
				SmartConsole.LogError($"Cannot spawn at spot index {index} as it is busy.");

				return;
			}
			
			m_Cubes[index] = InstantiateCube(index);
		}

		[Command("destroy_cube_index", "destroy the cube at spot index", MonoTargetType.Single)]
		private void DestroyAtIndex(int index)
		{
			if (index < 0 || index >= m_Spots.Length)
			{
				SmartConsole.LogError($"Index {index} is out of range.");

				return;
			}

			if (!m_Cubes[index])
			{
				SmartConsole.LogError($"Cannot destroy at spot index {index} as it is empty.");

				return;
			}

			Destroy(m_Cubes[index]);

			Debug.Log($"Destroyed cube.");
		}
	}
}