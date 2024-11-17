﻿using Memoirist_V2.WriterService.Models;

namespace Memoirist_V2.WriterService.RepoPattern;

public interface IWriterRepository {
	Task<Writer> GetWriterById (int id);
	Task<List<Writer>> GetListWriters();
	Task<List<Writer>> GetListFollowing(int id);
	Task<List<Writer>> GetListFollower(int id);
	
	Task<List<int>> GetListFollowingStoryId(int writerId);
	Task<List<int>> GetListStoryOfWriter(int writerId);	
	Task<Writer> GetWriterLogin();
	Task<Writer> UpdateWriter(int id, Writer updateItem);
	Task<Writer> AddStoryToList(int storyId, int writerId);
	Task<Writer> DeleteStoryFromList(int storyId, int writerId);
	Task FollowWriter(int idWriter, int idWriterFollow);
	Task FollowStory(int idWriter, int idStory);

}
