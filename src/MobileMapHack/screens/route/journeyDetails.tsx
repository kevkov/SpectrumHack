import React, {useEffect, useState, useContext} from 'react';
import {Card, CardItem, Content, Text, Left, Right, Body} from "native-base";
import { FlatList, StyleSheet } from 'react-native';
import { RouteInfo, JourneySettings } from '../../domain/types';
import { api } from '../../api';
import JourneyContext from '../../context/JourneyContext';

export const JourneyDetails = (props) => {
    const journeySettings = useContext<JourneySettings>(JourneyContext);
    const [routeInfoItems, setRouteInfoItems] = useState<RouteInfo[]>();
    
    useEffect(() => {
        var uri = "http://spectrummapapi.azurewebsites.net/api/map/routes/1/" + 
        journeySettings.showPollution + "/" +
        journeySettings.showSchools + "/" +
        journeySettings.startTime;
        
        console.log(journeySettings.showPollution);

        api<RouteInfo[]>(uri)
            .then(data => {
                console.log("api callback in journey details");
                setRouteInfoItems(data);
            });
        }, []);

    const GetHeaderStyle = (backgroundColourHex: string) => {
        return {
            backgroundColor: backgroundColourHex
        };
    }

    const styles = StyleSheet.create({
        headerText: {
            fontWeight: '600'
        },
        content: {
            padding: 10
        }
    });

    return (
        <Content style={styles.content}>
            <FlatList
                    data={routeInfoItems}
                    renderItem={datum =>
            <Card>
                <CardItem bordered style={GetHeaderStyle(datum.item.colorInHex)}>
                    <Text style={styles.headerText}>{datum.item.routeLabel}</Text>
                </CardItem>
                <CardItem>
                    <Left>
                        <Text>Air Quality Index: {datum.item.pollutionPoint}</Text>
                    </Left>
                    <Body>
                        <Text>Schools: {datum.item.schoolCount}</Text>
                    </Body>
                </CardItem>
                <CardItem>
                    <Left>
                        <Text>Distance: {datum.item.distance}</Text>
                    </Left>
                    <Body>
                        <Text>Travel Time: {datum.item.duration}</Text>
                    </Body>
                </CardItem>
                <CardItem>
                    <Left>
                        <Text style={styles.headerText}>Travel Cost: {datum.item.travelCost}</Text>
                    </Left>
                </CardItem>
            </Card>
            }/>
        </Content>
    )
};
