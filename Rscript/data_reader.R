require(dataframe);

let load_raw as function(file.csv) {
    let raw = read.csv.raw(file.csv, encoding = "utf8") :> rows;
    let names = raw[1] :> cells :> which(cell -> cell != "");
    let types = raw[2] :> cells;
    let data = list();

    print(names);
    print(unique(types));

    for(day in raw :> skip(2) :> projectAs(cells)) {
        let date = day[1];
        let regions = list();
        let dist;
        let i = 2; # [date], ....data
        
        for(name in names) {
            dist = list(name = name);

            for(n in 1:3) {
                dist[[types[i]]] <- as.numeric(day[i]);
                i <- i + 1;
            }

            regions[[name]] <- dist;
        }

        data[[date]] <- regions;
    }

    # str(data);

    data;
}

let get_all_dates as function(file.csv) {
    let raw = read.csv.raw(file.csv, encoding = "utf8") :> rows;
    let all <- [];

    for(r in raw :> skip(2)) {
        all <- all << (r :> cells)[1];
    }

    all;
}