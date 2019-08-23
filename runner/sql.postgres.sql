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

create table articles
(
	id bigserial not null
		constraint articles_pk
			primary key,
	name varchar(50),
	url varchar(255),
	created date,
	text varchar(2048),
	userid bigint not null
		constraint articles_users_id_fk
			references users
);

create index articles_userid_index
	on articles (userid);

create table comments
(
	id bigserial not null
		constraint comments_pk
			primary key,
	created date,
	text varchar(2048),
	articleid bigint not null
		constraint comments_articles_id_fk
			references articles,
	userid bigint not null
		constraint comments_users_id_fk
			references users
);

create index comments_userid_index
	on comments (userid);

create index comments_articleid_index
	on comments (articleid);

create table packages
(
	id bigserial not null,
	created date,
	init_system varchar(100),
	record_name varchar(100),
	record_created date,
	record_is_auth bool,
	record_username varchar(100),
	record_is_billed bool,
	record_provider varchar(100),
	record_is_done bool,
	record_partner varchar(100)
);

create unique index packages_id_uindex
	on packages (id);

alter table packages
	add constraint packages_pk
		primary key (id);

create index if not exists packages_init_system_index
	on packages (init_system);

create index if not exists packages_record_is_auth_index
	on packages (record_is_auth);

create index if not exists packages_record_is_billed_index
	on packages (record_is_billed);
