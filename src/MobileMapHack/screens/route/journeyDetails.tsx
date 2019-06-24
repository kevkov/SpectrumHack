import React, {useEffect, useState, useContext} from 'react';
import { Card, CardItem, Content, Text, Body, View, Accordion } from "native-base";
import { FlatList, StyleSheet } from 'react-native';
import { RouteInfo } from '../../domain/types';
import { api } from '../../api';
import JourneyContext from '../../context/JourneyContext';

export const JourneyDetails = () => {
    const {showPollution, showSchools, startTime} = useContext(JourneyContext);
    const [routeInfoItems, setRouteInfoItems] = useState<RouteInfo[]>();

    useEffect(() => {
        var uri = "http://spectrummapapi.azurewebsites.net/api/map/routes/1/" +
        showPollution + "/" +
        showSchools + "/" +
        startTime;

        console.log(showPollution);

        api<RouteInfo[]>(uri)
            .then(data => {
                console.log(`api callback in journey details ${uri}`);
                setRouteInfoItems(data);
            });
        }, [showPollution, showSchools, startTime]);

    const styles = StyleSheet.create({
        headerText: {
            fontWeight: '600'
        },
        content: {
            padding: 10
        },
        detailItem: {
            padding: 5
        }
    });

    return (
        <Content style={styles.content}>
            <FlatList
                    data={routeInfoItems}
                    keyExtractor={(item, index) => index.toString()}
                    renderItem={datum =>
            <Card>
                <CardItem bordered style={{backgroundColor: datum.item.colorInHex}}>
                    <Text style={styles.headerText}>{datum.item.routeLabel} ({datum.item.modeOfTransport})</Text>
                </CardItem>
                <CardItem style={{backgroundColor: '#eeeeee'}}>
                    <Body>
                        <View>
                            <Text style={styles.detailItem}>Green score: {datum.item.pollutionPoint}</Text>
                            <Text style={styles.detailItem}>Schools: {datum.item.schoolCount}</Text>
                            <Text style={styles.detailItem}>Distance: {datum.item.distance} miles</Text>
                            <Text style={styles.detailItem}>Average Air Quality: {datum.item.pollutionZone}</Text>
                            <Text style={styles.detailItem}>Travel time: {datum.item.duration}</Text>
                            <Text style={styles.detailItem}>Travel cost: £{datum.item.travelCost.toFixed(2)}</Text>

                            <Accordion dataArray={[{ title: "Calculation", content: "Lorem ipsum dolor sit amet" }]} expanded={0} />
                                                        <Text style={styles.detailItem}>
                                                            Green score is capped at 75 for cars.
                                                            Green Score = 
                                                            Start: 100 
                                                            Pollution: - ({datum.item.pollutionZone * 20}) 
                                                            Schools: - ({datum.item.schoolCount} * 40) = 
                                                            {datum.item.pollutionPoint}
                                                            
                                                            Cost = 
                                                            Start: 10
                                                            Green Factor: - {datum.item.pollutionPoint} / 10 
                                                            Distance: * {datum.item.distance} (miles) = 
                                                            {datum.item.travelCost.toFixed(2)}
                                                        </Text>
                        </View>
                    </Body>
                </CardItem>
            </Card>
            }/>
        </Content>
    )
};
