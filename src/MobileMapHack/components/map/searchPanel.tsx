import React, {useContext, useState} from "react";
import {Animated} from "react-native";
import {Button, Card, CardItem, Icon, Label, Picker} from "native-base";
import {Journey, theOneGoodJourney} from "../../domain/types";
import {fromNullable} from "fp-ts/lib/Option";
import JourneyContext from "../../context/JourneyContext";
import {useSlideInOutAnimation} from "../../hooks/animation";
import { GooglePlacesAutocomplete } from 'react-native-google-places-autocomplete';

export const SearchPanel = (props:{show:boolean, journey:Journey | null}) => {

    const {show} = props;
    const [time, setTime] = useState("08:00");
    const {setStartTime, setJourney} = useContext(JourneyContext);

    let top = useSlideInOutAnimation(show, 10, -290);

    const journeyDetails = fromNullable(props.journey)
        .map(j => ({startName: j.startName, endName: j.endName, startTime: j.startTime}))
        .getOrElse({startName: "", endName: "", startTime: "08:00"});
    const range = new Array(47).fill(0);
    let queryParams = {
        key: '',
        language: 'en', // language of the results
        types: 'geocode' // '(cities)'
    };
    let autoCompleteStyles = {
        textInputContainer: {
            flex: 4,
            borderTopWidth: 1,
            borderBottomWidth: 1,
            borderWidth: 1,
            borderRadius: 5,
            borderColor: "#CCCCCC",
            // height: undefined,
            backgroundColor: "#FFFFFF",
        },
        textInput: {
            borderWidth: 0
        }
    };
    return (
        <Animated.View style={{top: top, position: 'absolute', right: 10, left: 10}}>
            <Card style={{borderRadius: 5}}>
                <CardItem>
                    <GooglePlacesAutocomplete
                        placeholder="From"
                        minLength={2}
                        autoFocus={false}
                        listViewDisplayed="false"
                        fetchDetails={true}
                        renderDescription={row => row.description} // custom description render
                        getDefaultValue={() => journeyDetails.startName}
                        query={queryParams}
                        styles={autoCompleteStyles}
                        onPress={(rowData, details) => console.log(details.geometry.location)}
                    />
                </CardItem>
                <CardItem>
                    <GooglePlacesAutocomplete
                        placeholder="To"
                        minLength={2}
                        autoFocus={false}
                        listViewDisplayed="false"
                        fetchDetails={true}
                        renderDescription={row => row.description} // custom description render
                        getDefaultValue={() => journeyDetails.endName}
                        query={queryParams}
                        styles={autoCompleteStyles}
                    />
                </CardItem>
                <CardItem>
                    <Label style={{marginRight: 5}}>Time</Label>
                    <Picker
                        mode="dropdown"
                        onValueChange={(value) => setTime(value)}
                        selectedValue={time}
                    >
                        {range.map((_, index) => {
                            const hour = Math.floor(index / 2);
                            const mins = index % 2 == 0 ? "00" : "30";
                            const timeOption = `${hour < 10 ? "0" + hour : hour.toString()}:${mins}`;
                            return (<Picker.Item label={timeOption} value={timeOption} key={timeOption}/>);
                        })}
                    </Picker>
                    <Button primary style={{width:50, height:50, borderRadius:25, alignItems:"center", justifyContent:"center"}}
                        onPress={() => {
                            setStartTime(time);
                            setJourney(theOneGoodJourney);
                        }}>
                        <Icon name="search" type="MaterialIcons" />
                    </Button>
                </CardItem>
            </Card>
        </Animated.View>
    )
};
