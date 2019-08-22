create table test
(
	counter int,
	name nvarchar(32),
	type nvarchar(32),
	count bigint,
	info nvarchar(max),
	id bigint not null
		constraint test_pk
			primary key
);

create table users
(
	id bigint not null
		constraint users_pk
			primary key,
	name nvarchar(50),
	created date,
	url nvarchar(255)
);

create index users_name_index
	on users (name);

create table articles_comments
(
	id bigint not null
		constraint articles_comments_pk
			primary key IDENTITY(1,1),
	name nvarchar(50),
	url nvarchar(255),
	created date,
	text nvarchar(2048),
	userid bigint not null
		constraint articles_comments_users_id_fk
			references users,
	comments nvarchar(max)
);

alter table articles_comments
add vUserId as json_value(comments,'$.UserId');

create index articles_comments_commenets_index on articles_comments  (vUserId);

create index articles_comments_userid_index
	on articles_comments (userid);
