create table test
(
	counter int,
	name varchar(32),
	type varchar(32),
	count bigint,
	info jsonb,
	id bigserial not null
		constraint test_pk
			primary key
);

create table users
(
	id bigserial not null
		constraint users_pk
			primary key,
	name varchar(50),
	created date,
	url varchar(255)
);

create index users_name_index
	on users (name);

create table articles_comments
(
	id bigserial not null
		constraint articles_comments_pk
			primary key,
	name varchar(50),
	url varchar(255),
	created date,
	text varchar(2048),
	userid bigint not null
		constraint articles_comments_users_id_fk
			references users,
	comments jsonb
);

create index articles_comments_userid_index
	on articles_comments (userid);

CREATE INDEX articles_comments_comments_index ON articles_comments USING GIN (comments);
CREATE INDEX articles_comments_comments_1_index ON articles_comments USING GIN ((comments -> 'UserId'));