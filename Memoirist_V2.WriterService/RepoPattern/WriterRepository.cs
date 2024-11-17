﻿using AutoMapper;
using Memoirist_V2.WriterService.DataContext;
using Memoirist_V2.WriterService.Models;
using Memoirist_V2.WriterService.Models.DTO;
using Memoirist_V2.WriterService.RepoPattern.RabbitMess;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Memoirist_V2.WriterService.RepoPattern;

public class WriterRepository : IWriterRepository {
	public readonly WriterDbContext dbContext;
	public readonly IRabbitWriterRepository rabbitWriterRepository;

	public WriterRepository(WriterDbContext dbContext, IRabbitWriterRepository rabbitWriterRepository) {
		this.dbContext = dbContext;
		this.rabbitWriterRepository = rabbitWriterRepository;
	}

	public async Task<List<Writer>> GetListFollower(int id) {
		await SaveNewWriter();
		var writerDomain = await dbContext.Writers.FirstOrDefaultAsync(x => x.WriterId == id);	
		var listFollower = new List<Writer>();
		foreach(var item in writerDomain.ListFollower) {
			var flagItem = await dbContext.Writers.FirstOrDefaultAsync(x => x.WriterId == item);
			listFollower.Add(flagItem);
		}
		return listFollower;
	}

	public async Task<List<Writer>> GetListFollowing(int id) {
		await SaveNewWriter();
		var writerDomain = await dbContext.Writers.FirstOrDefaultAsync(x => x.WriterId == id);
		var listFollowing = new List<Writer>();
		foreach(var item in writerDomain.ListFollowing) {
			var flagItem = await dbContext.Writers.FirstOrDefaultAsync(x => x.WriterId == item);
			listFollowing.Add(flagItem);
		}
		return listFollowing;
	}

	public async Task<List<Writer>> GetListWriters() {
		await SaveNewWriter();
		var writerDomain = await dbContext.Writers.ToListAsync();
		return writerDomain;
	}

	public async Task<Writer> GetWriterById(int id) {
		await SaveNewWriter();
		var writerDomain = await dbContext.Writers.FirstOrDefaultAsync(x => x.WriterId == id);
		if(writerDomain == null) {
			return null;
		} else {
			rabbitWriterRepository.SendListFollowingStoryIdOfWriter(writerDomain.ListFollowingStoryId, "FollowingStoryIdQueue");//sent list following story when get profile
			rabbitWriterRepository.SendListStoryOfWriter(writerDomain.ListStoryId, "StoryOfWriterQueue");// sent list story user when get profile
			return writerDomain;
		}
	}
	public async Task<Writer> GetWriterLogin() {
		var writerItem = await rabbitWriterRepository.ReceiveMessageObject("UserLoginMess");
		return writerItem;
	}

	public async Task<Writer> UpdateWriter(int id, Writer updateItem) {
		await SaveNewWriter();
		var DomainItem = await dbContext.Writers.FirstOrDefaultAsync(x => x.WriterId == id);
		if(DomainItem == null) {
			return null;
		} else {
			if(updateItem.WriterAvatar != "string") {
				DomainItem.WriterAvatar = updateItem.WriterAvatar;
			}
			if(updateItem.WriterBio != "string") {
				DomainItem.WriterBio = updateItem.WriterBio;
			}
			if(updateItem.WriterFullname != "string") {
				DomainItem.WriterFullname = updateItem.WriterFullname;
			}
			if(updateItem.WriterUsername != "string") {
				DomainItem.WriterUsername = updateItem.WriterUsername;
			}
			await dbContext.SaveChangesAsync();
		}
		return DomainItem;
	}
	public async Task<Writer> UpdateWriterWhenAddStory(int id, int storyId) {
		var domainItem = await dbContext.Writers.FirstOrDefaultAsync(x => x.WriterId == id);
		if(domainItem != null) {
			domainItem.ListStoryId.Add(storyId);
		} else {
			return null;
		}
		await dbContext.SaveChangesAsync();
		return domainItem;
	}

	public async Task SaveNewWriter() {
		var writerList = await rabbitWriterRepository.ReceiveMessage("UserRegisterMess");
		dbContext.Writers.AddRange(writerList);
		await dbContext.SaveChangesAsync();
	}

	public async Task FollowWriter(int idWriter, int idWriterFollow) {
		var writer = await dbContext.Writers.FirstOrDefaultAsync(x => x.WriterId == idWriter);
		var writerFollow = await dbContext.Writers.FirstOrDefaultAsync(x => x.WriterId == idWriterFollow);

		if(writer == null || writerFollow == null) {
			throw new InvalidOperationException("Writer or writer to follow not found.");
		} 
		if(!writer.ListFollowing.Contains(idWriterFollow)) {
			writer.ListFollowing.Add(idWriterFollow);
			writerFollow.ListFollower.Add(idWriter);
		} else {
			writer.ListFollowing.Remove(idWriterFollow);
			writerFollow.ListFollower.Remove(idWriter);
		}
		await dbContext.SaveChangesAsync();
	}
	public async Task FollowStory(int idWriter, int idStory) {
		var writerDomain = await dbContext.Writers.FirstOrDefaultAsync(x => x.WriterId == idWriter);
		if(writerDomain == null) {
			throw new InvalidOperationException("Writer not found.");
		}
		if(!writerDomain.ListFollowingStoryId.Contains(idStory)) {
			writerDomain.ListFollowingStoryId.Add(idStory);
		} else {
			writerDomain.ListFollowingStoryId.Remove(idStory);
		}
		await dbContext.SaveChangesAsync();
	}
	public async Task<List<int>> GetListFollowingStoryId(int writerId) {
		//Tim kiem writer dua tren id
		var item = await dbContext.Writers.FirstOrDefaultAsync(x => x.WriterId == writerId);
		if(item == null) {
			return null;
		}
		if(item.ListFollowingStoryId == null) {
			return null;
		}
		//gui danh sach cac story writer follow qua cho Story Service
		rabbitWriterRepository.SendListFollowingStoryIdOfWriter(item.ListFollowingStoryId, "FollowingStoryIdQueue");
		return item.ListFollowingStoryId;
	}
	public async Task<List<int>> GetListStoryOfWriter(int writerId) {
		var item = await dbContext.Writers.FirstOrDefaultAsync(x => x.WriterId == writerId);
		if(item == null) {
			return null;
		}
		if(item.ListStoryId == null) {
			return null;
		}
		rabbitWriterRepository.SendListStoryOfWriter(item.ListStoryId, "StoryOfWriterQueue");
		return item.ListStoryId;

	}

	public async Task<Writer> UpdateWriterWhenDeleteStory(int id, int storyId) {
		var item = await dbContext.Writers.FirstOrDefaultAsync(x => x.WriterId == id);
		if(item == null) {
			return null;
		}
		if(item.ListFollowingStoryId.Contains(storyId)) {
			item.ListFollowingStoryId.Remove(storyId);
		}
		if(item.ListStoryId.Contains(storyId)){
			item.ListStoryId.Remove(storyId);
		}
		return item;
	}
}
