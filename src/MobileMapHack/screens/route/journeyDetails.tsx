import React, {useEffect, useState} from 'react';
import {Card, CardItem, Content, Text, Left, Right, Body} from "native-base";
import { FlatList, StyleSheet } from 'react-native';
import { RouteInfo } from '../../domain/types';

export const JourneyDetails = (props) => {

    const [routeInfoItems, setRouteInfoItems] = useState<RouteInfo[]>();

    function api<T>(url: string): Promise<T> {
        return fetch(url)
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.statusText)
                }
                return response.json() as Promise<T>
            })
    }

    useEffect(() => {
        console.log("Inside useEffect on journey details");

        api<RouteInfo[]>("http://spectrummapapi.azurewebsites.net/api/map/routes/1/true/true/09:00")
            .then(data => {
                console.log("calling  route info api");
                setRouteInfoItems(data);
            });
        }, [props.showPollution, props.showSchools, props.startTime]);

    const GetHeaderStyle = (backgroundColourHex: string) => {
        const blue = backgroundColourHex.substring(2,4);
        const green = backgroundColourHex.substring(4,6);
        const red = backgroundColourHex.substring(6);

        return {
            backgroundColor: '#' + red + green + blue
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
