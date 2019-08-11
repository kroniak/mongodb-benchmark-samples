create table test
(
	counter int,
	name varchar(32),
	type varchar(32),
	count bigint,
	info jsonb,
	id serial not null
		constraint test_pk
			primary key
);

